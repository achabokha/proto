export interface Transaction {
    reference: string,
    txtType: string,
    txnCode: string,
    status: string,
    isAmountKnown: boolean,
    originalAmount: number,
    originalCurrencyCode: string,
    destinationAmount: number,
    destinationCurrencyCode: string,
    dateCreated: any,
}