export class BaseElement {
	children = [];

	token = null;

	constructor(token) {
		this.token = token;
	}

	toBBCode() {
		throw new Error('Not implemented');
	}
}

export class HeadingElement extends BaseElement {
	heading2Size = {
		1: 5,
		2: 4,
		3: 3,
		4: 3,
		5: 3,
	};

	toBBCode() {
		const text = this.children.map((item) => item.toBBCode()).join('');
		return [`[size=${this.heading2Size[this.token.depth]}][b]${text}[/b][/size]`];
	}
}

export class ListElement extends BaseElement {
	toBBCode() {
		const listItems = this.children.map((item) => item.toBBCode());

		return [
			"[list=1]",
			...listItems,
			"[/list]",
		].flat();
	}
}

export class ListItemElement extends BaseElement {
	toBBCode() {
		const content = this.children.map((item) => item.toBBCode());
		return [`[*] ${content.flat().join(' ')}`].flat();
	}
}

export class TextElement extends BaseElement {
	toBBCode() {
		if (this.children.length > 0)
			return [this.children.map((item) => item.toBBCode()).join(' ')];

		return [this.token.text].flat();
	}
}

export class LinkElement extends BaseElement {
	toBBCode() {
		const text = this.children.map((item) => item.toBBCode()).join(' ');

		// Anchors doesn't work in BBCode
		if (this.token.href?.startsWith('#'))
			return [text];

		return [`[url=${this.token.href}]${text}[/url]`].flat();
	}
}

export class CodeSpanElement extends BaseElement {
	toBBCode() {
		return [`[font=Courier New][color=#ffff00]${this.token.text}[/color][/font]`].flat();
	}
}

export class StrongElement extends BaseElement {
	toBBCode() {
		const text = this.children.map((item) => item.toBBCode()).join('');

		return [`[b]${text}[/b]`].flat();
	}
}

export class ParagraphElement extends BaseElement {
	toBBCode() {
		const text = this.children.map((item) => item.toBBCode()).join('');

		return [text.replace(/\n/g, ' ')].flat();
	}
}

export class SpaceElement extends BaseElement {
	toBBCode() {
		return [''];
	}
}

export class LineElement extends BaseElement {
	toBBCode() {
		return ['', '[line]', ''].flat();
	}
}

export class TableElement extends BaseElement {
	headerTokens = [];
	rowsTokens = [];

	toBBCode() {
		const header = this.headerTokens.map(
			(headerItem) => headerItem.map(tok => tok.toBBCode()).join('')
		);

		const rows = this.rowsTokens.map(
			(row) => row.map(
				col => col.map(tok => tok.toBBCode()).join('')
			)
		);

		const formattedRows = rows.map((row) => {
			const cols = [];
			for (let i = 0; i < header.length; i++)
				cols.push(`[b]${header[i]}:[/b] ${row[i]}`);
			return `[*]${cols.join('\n')}`;
		});

		return [
			'[list]',
			formattedRows.join('\n'),
			'[/list]',
		];
	}
}

export class HtmlElement extends BaseElement {
	toBBCode() {
		return [];
	}
}

export class BLockquoteElement extends BaseElement {
	toBBCode() {
		const text = this.children.map((item) => item.toBBCode()).join('');

		return [
			`[quote]${text}[/quote]`,
		];
	}
}

export class CodeElement extends BaseElement {
	toBBCode() {
		return [
			`[code]${this.token.text}[/code]`,
		];
	}
}

export class BBCodeElement extends BaseElement {
	toBBCode() {
		return [this.token.raw]
	}
}
