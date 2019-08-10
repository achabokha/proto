const locations: string[] = ['New York',
	'Los Angeles',
	'Chicago',
	'Houston', 'Phoenix',
	'Philadelphia',
	'San Antonio',
	'San Diego',
	'Dallas',
	'San Jose',
	'Austin',
	'Jacksonville',
	'Fort Worth',
	'Columbus',
	'San Francisco',
	'Charlotte',
	'Indianapolis',
	'Seattle',
	'Denver',
	'Washington',
	'Boston',
	'El Paso',
	'Detroit'];

function getRandomInt(min, max) {
	min = Math.ceil(min);
	max = Math.floor(max);
	return Math.floor(Math.random() * (max - min + 1)) + min;
}

export function getLocations(days: number): Promise<any[]> {
	return new Promise((resolve, reject) => {
		const inviteCount = (200 * days) + (300 * days) + (600 * days);
		resolve(locations.map(locName => {
			const t: any = {
				location: locName,
				shelterUserCount: getRandomInt(1 * days, 10 * days),
				facebookUserCount: getRandomInt(100 * days, 300 * days),
				googleUserCount: getRandomInt(200 * days, 600 * days),
			};
			t.inviteCount = inviteCount;

			return t;
		}));
	});
}

