export interface Application {
    applicationId: string;
    dateCreated: any;
    dateModified: any;
    userId: string;
    accountType: string;
    reference: string;
    statusString: string;
    comments: string;

    documets: any;
    firstName: string; // mandatory
    lastName: string; // mandatory
    currencyCodeString: string; // USD, EUR
    title: string; // Dr, Mr, Mrs, Ms
    dateOfBirth: string; // (yyyy-MM-dd)
    nationality: string; // 3 letter (ISO 3166-1)
    gender: string; // (optional) (male-female)
    maritalStatus: string; // (optional) m-s-w-d
    phone: string; // (optional)
    email: string; // (optional)
}
