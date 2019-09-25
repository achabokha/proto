export interface Product {
	id: string;
	title: string;
	price: number;
	category: string;
	imageUrl: string;
	description: string;
}

export function mockProduct(): Product {
	return {
		id: '1',
		title: 'Clean code',
		price: 35.5,
		category: "1",
		imageUrl: "image.png",
		description: 'clean code is a ...'
	}
}