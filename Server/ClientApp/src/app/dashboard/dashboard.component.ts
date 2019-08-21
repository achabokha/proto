import { Component } from '@angular/core';
import { map } from 'rxjs/operators';
import { Breakpoints, BreakpointObserver } from '@angular/cdk/layout';


@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent {
  isVisible = true;
  columnSplit = '33% 33% 33%';
  cards = [{ imgSrc: 1, title: 'Card 1', cols: 1, rows: 1 },
  { imgSrc: 2, title: 'Card 2', cols: 1, rows: 1 },
  { imgSrc: 3, title: 'Card 3', cols: 1, rows: 1 },
  { imgSrc: 4, title: 'Card 4', cols: 1, rows: 1 },
  { imgSrc: 5, title: 'Card 5', cols: 1, rows: 1 },
  { imgSrc: 6, title: 'Card 6', cols: 1, rows: 1 },
  { imgSrc: 7, title: 'Card 7', cols: 1, rows: 1 },
  ];
  /** Based on the screen size, switch from standard to one column per row */

  constructor(private breakpointObserver: BreakpointObserver) {
    this.breakpointObserver.observe([Breakpoints.Small, Breakpoints.HandsetLandscape, Breakpoints.HandsetPortrait, Breakpoints.Large, Breakpoints.XLarge, Breakpoints.Medium]).subscribe(matches => {
      if (matches.breakpoints[Breakpoints.Large]) {
        this.columnSplit = '25% 25% 25% 25%';
      } else if (matches.breakpoints[Breakpoints.XLarge]) {
        this.columnSplit = '20% 20% 20% 20%';
      } else if (matches.breakpoints[Breakpoints.Medium]) {
        this.columnSplit = '33% 33% 33%';
      } else if (matches.breakpoints[Breakpoints.Small]) {
        this.columnSplit = '50% 50%';
      } else {
        this.columnSplit = '100%';
      }
    });

  }
}
