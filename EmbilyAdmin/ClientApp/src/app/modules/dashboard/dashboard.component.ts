import { Component, OnInit, ViewChild } from '@angular/core';
import { DashboardService } from "../../services/dashboard.service";
import { Observable, timer } from 'rxjs';
import { Dashboard } from '../../models/dashboard';
import { FormGroup } from '@angular/forms';


@Component({
    selector: 'app-dashboard',
    templateUrl: './dashboard.component.html',
    styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
    dashboard: Dashboard;
    transactions: any;
    applications: any;
    datetimeUTC: string = new Date().toUTCString();


    // Line
    modelTransactions: any = 2;
    modelTransactionsPie: any = 3;
    modelApplications: any = 3;
    currency: any = "USD";
    hoursTransactions: any = 168;
    hoursTransactionsPie: any = 720;
    hoursApplications: any = 720;

    private options = {
        scales: {
            yAxes: [{
                id: 'left-y-axis',
                type: 'linear',
                position: 'left',
                ticks: {
                    beginAtZero: true
                }
            }, {
                id: 'right-y-axis',
                type: 'linear',
                position: 'right',
                ticks: {
                    beginAtZero: true
                }
            }]
        }
    };

    private optionsApp = {
        scales: {
            yAxes: [{
                id: 'left-y-axis',
                type: 'linear',
                position: 'left',
                ticks: {
                    beginAtZero: true
                }
            }]
        }
    };

    chartTransactionsData: Array<any> = [];
    chartApplicationsData: Array<any> = [];

    transactionsChartLabels: any = [];
    applicationsChartLabels: any = [];

    lineChartOptions: any = {
        //animation: false,
        //responsive: true,
        //maintainAspectRatio: false
    };
    
    lineChartColors: any = [
        {
            fill: false,
            //borderDash: [5, 5],
            borderColor: "#9C27B0",
            pointBorderColor: "#9C27B0",
            pointBackgroundColor: "#FFF",
            pointBorderWidth: 2,
            pointHoverBorderWidth: 2,
            pointRadius: 4,
        },
        {
            borderColor: 'rgba(0,128,0,1)',
            pointBorderColor: 'rgba(0,128,0,1)',
            pointBackgroundColor: "#FFF",
            pointBorderWidth: 2,
            pointHoverBorderWidth: 2,
            pointRadius: 4,

            backgroundColor: 'rgba(108, 226, 108, 0.8)',                                
            pointHoverBackgroundColor: '#fff',
            pointHoverBorderColor: 'rgba(148,159,177,0.8)'
        },
    ];

    lineChartLegend = true;
    lineChartType = 'line';

    // Pie
    public pieChartLabels: string[] = [
        'less 500',
        'more 500 and less 1,000',
        'more 1,000 and less 2,500',
        'more 2,500 and less 5,000',
        'more 5,000'
    ];
    public pieChartData: number[];// = [75, 40, 115, 200, 10];
    public pieChartType: string = 'pie';

    //private pieoptions: any = {
    //    legend: { position: 'right' }
    //}

    constructor(private dashboardService: DashboardService) { }      

    // events
    public chartClicked(e: any): void {
        console.log(e);
    }

    public chartHovered(e: any): void {
        console.log(e);
    }

    ngOnInit(): void {
        timer(0, 1000).subscribe(() => this.datetimeUTC = new Date().toUTCString());

        this.ngAfterViewInit();
        this.applyFilterTransactions(168, 'USD');
        this.applyFilterApplications(720);

        this.applyFilterDonutAmount(720);
    }

    applyFilterTransactions(hours, currency) {

        if (hours) {
            this.hoursTransactions = hours;
        }
        if (currency) {
            this.currency = currency;
        }
        this.transactionsLoad();
    } 
    
    applyFilterApplications(hours) {
        if (hours) {
            this.hoursApplications = hours;
        }

        this.applicationsLoad();
    }

    applyFilterDonutAmount(hours) {

        if (hours) {
            this.hoursTransactionsPie = hours;
        }

        this.dashboardService.GetTransactionDistribution(this.hoursTransactionsPie).subscribe(r => {
            this.pieChartData = r.pieData;
        });
    }

    //applyFilterDonutCount(event, hours) {
    //    event.target.parentElement.getElementsByClassName("selected")[0].className = "link"
    //    event.target.className = "link selected";
    //    //this.donutChartCount.drawChart(this.optionsDonutCount.filter(hours));
    //}

    transactionsLoad() {
        this.dashboardService.GetTransactions(this.hoursTransactions, this.currency).subscribe(r => {
            this.transactions = r;
            this.chartTransactionsData = r.chartData;
            this.transactionsChartLabels = r.lineChartLabels;

        });
    }
    applicationsLoad() {
        this.dashboardService.GetApplications(this.hoursApplications).subscribe(r => {
            this.applications = r;
            this.chartApplicationsData = r.chartData;
            this.applicationsChartLabels = r.lineChartLabels;
        });
    }

    ngAfterViewInit(): void {
        this.dashboardService.GetAll().subscribe(r => {
            this.dashboard = r;                      
        });
    }
}
