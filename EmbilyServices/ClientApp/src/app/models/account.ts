export interface Account {
    accountId: string;
    accountType: string;
    accountNumber: string,
    accountName: string;
    balance: number | null;
    currencyCode: string,
    currencyCodeString: string,
    providerAccountNumber: string;
    cardNumber: string;
    accountStatus: string;
    accountStatusString: string;
    userId: string;
}
