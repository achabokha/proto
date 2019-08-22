import { Component, OnInit } from '@angular/core';
import { BreakpointObserver, Breakpoints } from '@angular/cdk/layout';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: ['./search.component.scss']
})
export class SearchComponent implements OnInit {

  columnSplit = '33% 33% 33%';
  rowSplit = '33vh!';
  cards = [
  { imgSrc: 1, title: 'Card 1', cols: 1, rows: 1, color: '#eb4034' },
  { imgSrc: 2, title: 'Card 2', cols: 1, rows: 1, color: '#64b5a6'  },
  { imgSrc: 3, title: 'Card 3', cols: 1, rows: 1, color: '#8e83c9'  },
  { imgSrc: 4, title: 'Card 4', cols: 1, rows: 1, color: '#cfcf74'  },
  { imgSrc: 5, title: 'Card 5', cols: 1, rows: 1, color: '#74e3a9'  },
  { imgSrc: 6, title: 'Card 6', cols: 1, rows: 1, color: '#e374e1'  },
  { imgSrc: 7, title: 'Card 7', cols: 1, rows: 1, color: '#c9ae5d'  },
  ];
  cardList = this.cards.slice(0, 4);

  constructor(private breakpointObserver: BreakpointObserver) {
    this.breakpointObserver.observe([Breakpoints.Small, Breakpoints.HandsetLandscape, Breakpoints.HandsetPortrait, Breakpoints.Large, Breakpoints.XLarge, Breakpoints.Medium]).subscribe(matches => {
      if (matches.breakpoints[Breakpoints.Large]) {
        this.columnSplit = '25% 25% 25% 25%';
        this.rowSplit = '33vh!';
      } else if (matches.breakpoints[Breakpoints.XLarge]) {
        this.columnSplit = 'repeat(4, 25%)';
        this.rowSplit = 'auto!';
      } else if (matches.breakpoints[Breakpoints.Medium]) {
        this.columnSplit = 'repeat(5, 20%)';
        this.rowSplit = 'auto!';
      } else if (matches.breakpoints[Breakpoints.Small]) {
        this.columnSplit = '50% 50%';
        this.rowSplit = 'auto!';
      } else {
        this.columnSplit = '50% 50%';
        this.rowSplit = '33vh!';
      }
    });
  }

  ngOnInit() {
  }

}
