import { Injectable } from '@angular/core';
import { Account, Application, Transaction } from "../models";
import { ITheme } from '../themes';

@Injectable()
export class DataService {

    theme: ITheme;

    isAffiliate: boolean = false;

    affiliateAccounts: Account[] | null = null;

    affiliateTransactions: null;

    loadTransactions: Transaction[] | null = null;

    allTransactions: Transaction[] | null;

    get promoToken(): string {
        return this.getStoredItem('promo-token') || '';
    }

    set promoToken(token: string) {
        this.setStoredItem('promo-token', token);
    }

    public accounts: Account[] | null = null;

    public applications: Application[] | null = null;

    public stage: string | null = null;

    public posts: any;

    public featured: Array<any> = [];

    public cardSpinned = false;

    constructor() {
    }

    clean(): any {
        this.accounts = null;
        this.applications = null;
        this.stage = null;
    }

    cleanAll() {
        this.cardSpinned = false;
        this.posts = null;
        this.featured = [];
        this.cleanTransactions();
        this.clean();
    }

    cleanTransactions() {
        this.allTransactions = null;
        this.affiliateTransactions = null;
        this.loadTransactions = null;
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
