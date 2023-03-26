import fs from 'fs';
import { marked } from 'marked';
import path from 'path';
import { rawRepositoryURL, repoRoot, repositoryFileURL, repositoryURL } from './configs.js';
import { BBCodeElement, BLockquoteElement, CodeElement, CodeSpanElement, HeadingElement, HtmlElement, LineElement, LinkElement, ListElement, ListItemElement, ParagraphElement, SpaceElement, StrongElement, TableElement, TextElement } from './textElements.js';

let fileAbsolutePath;

const typeToClass = {
	heading: HeadingElement,
	paragraph: ParagraphElement,
	space: SpaceElement,
	hr: LineElement,
	table: TableElement,
	html: HtmlElement,
	list: ListElement,
	list_item: ListItemElement,
	text: TextElement,
	link: LinkElement,
	codespan: CodeSpanElement,
	strong: StrongElement,
	blockquote: BLockquoteElement,
	code: CodeElement,
	bbcode: BBCodeElement, // custom
}

function lexFile() {
	const markdownPath = process.argv[2];
	if (!markdownPath) {
		console.error('Usage: npm run md2nexus <file name>');
		process.exit(1);
	}

	if (!fs.existsSync(markdownPath)) {
		console.error(`File "${markdownPath}" does not exists.`);
		process.exit(1);
	}

	fileAbsolutePath = path.resolve(process.argv[2], '..');

	return marked.lexer(fs.readFileSync(markdownPath).toString());
}

function makeNexusOnlyToken(lines, startAt) {
	const nexusContent = [];

	let idx = startAt;
	while (idx < lines.length && lines[idx].indexOf('nexus-only') < 0)
		idx++;

	idx++;

	while (idx < lines.length && lines[idx].indexOf('-->') < 0) {
		nexusContent.push(lines[idx]);
		idx++
	}

	return {
		token: {
			type: 'bbcode',
			raw: nexusContent
				.join('\n')
				.replace(/@@@RawRepositoryURL@@@/g, rawRepositoryURL)
				.replace(/@@@RepositoryFileURL@@@/g, repositoryFileURL)
				.replace(/@@@RepositoryURL@@@/g, repositoryURL),
		},
		stopAt: idx + 1,
	};
}

function makeNexusOnlyTokens(baseToken) {
	const lines = baseToken.raw.split('\n')
		.map((v) => v.trim())
		.filter((v) => !!v);

	const tokens = [];
	let continueFrom = 0;
	let processed
	do {
		processed = makeNexusOnlyToken(lines, continueFrom);
		continueFrom = processed.stopAt;
		if (processed.token)
			tokens.push(processed.token);
	} while (processed.stopAt < lines.length);

	return tokens;
}

function loadNexusMdLex() {
	let isSkipping = false;
	const lexer = lexFile()
		.reduce((acc, token) => {
			if (token.type === 'html') {
				if (token.raw.indexOf('nexus-only') >= 0) {
					acc.push(...makeNexusOnlyTokens(token));
				}

				if (token.raw.indexOf('nexus-enable') >= 0) {
					isSkipping = false;
					return acc;
				}

				if (token.raw.indexOf('nexus-disable') >= 0) {
					isSkipping = true;
					return acc;
				}
			}

			if (isSkipping)
				return acc;

			acc.push(token);
			return acc;
		}, []);

	return lexer;
}

/**
 *
 * @param {marked.TokensList[0]} token
 */
function parseToken(token) {
	try {
		const element = new typeToClass[token.type](token);

		if (token.tokens)
			element.children.push(...token.tokens.map(parseToken));

		if (token.type === 'list')
			element.children.push(...token.items.map(parseToken));

		if (token.type === 'table') {
			element.headerTokens = token.header.map(
				headerItem => headerItem.tokens.map(parseToken)
			);
			element.rowsTokens = token.rows.map(
				row => row.map(rowCol => rowCol.tokens.map(parseToken))
			);
		}

		if (token.type === 'link') {
			if (!token.href.startsWith('#') && token.href.indexOf('://') < 0) {
				const filePath = path.relative(repoRoot, path.resolve(fileAbsolutePath, token.href));
				token.href = `${repositoryFileURL}/${filePath}`;
			}
		}

		return element;
	} catch (error) {
		console.error('Failed to parse token', token, error);
		process.exit(1);
	}
}

try {
	const lexer = loadNexusMdLex();
	const elementTree = lexer.map(parseToken);

	let finalText = [];
	elementTree.forEach((tree) => {
		finalText.push(...tree.toBBCode());
	});

	console.log(finalText.join('\n'));
} catch (error) {
	console.error('Could not convert markdown to Nexus BBCode.');
	console.error(error);
	process.exit(1);
}
