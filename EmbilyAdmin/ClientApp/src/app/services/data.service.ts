import { Injectable } from '@angular/core';
import { Account, Application } from "../models";

@Injectable()
export class DataService {

    public applications: Application[] | null = null;
    public accounts: any;
    public selectApp: any;
    public address: any;
    public shippingAddress: any;
    public cardOrders: any;
    public isAdmin: any;

    public users: any;
    public user: any;

    public affiliateInvite: any;
    public affiliateUsers: any;
    public affiliateTokens: any;

    public programs: any;


    get environmentName(): string {
        return "Sandbox";
    }

    constructor() {
    }

    cleanTransactions() {
        this.affiliateInvite = null;
        this.affiliateUsers = null;
        this.affiliateTokens = null;
    }

    clean(): any {
        this.accounts = null;
        this.applications = null;
    }

    get promoToken(): string {
        return this.getStoredItem('promo-token') || '';
    }

    set promoToken(token: string) {
        this.setStoredItem('promo-token', token);
    }

    setStoredItem(name: string, obj: any) {
        if (typeof window !== 'undefined') { // hack for server side rendering, server side dose not have window object, thus storage --
            localStorage.setItem(name, obj);
        }
    }

    getStoredItem(name: string): string | null {
        if (typeof window !== 'undefined') { // hack for server side rendering, server side dose not have window object, thus storage --
            return localStorage.getItem(name);
        }
        return null;
    }
}
