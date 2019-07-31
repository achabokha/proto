import { Transaction } from "./transaction";
import { CryptoAddress } from "./cryptoAddress";

export interface Account {
    accountId: string;
    accountNumber: number,
    accountName: string;
    balance: number;
    currencyCode: string;
    currencyCodeString: string;
    accountTypeString: string;
    transactions: Transaction[];
    cryptoAddreses: CryptoAddress[];
    accountStatusString: string;
    accountStatus: string;
}
