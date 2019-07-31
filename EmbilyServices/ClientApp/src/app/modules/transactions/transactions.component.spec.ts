/// <reference path="../../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { TransactionsComponent } from './transactions.component';

let component: TransactionsComponent;
let fixture: ComponentFixture<TransactionsComponent>;

describe('transactions component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ TransactionsComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(TransactionsComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});