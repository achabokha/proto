import { Component, OnInit, ViewChild, TemplateRef } from '@angular/core';

import { MatDialog, MatDialogConfig } from '@angular/material';

import * as d3 from 'd3';
import { LocationDataService } from '../services/location-data.service';


interface Location {
  location: string;
  inviteCount: number;
  shelterUserCount: number;
  facebookUserCount: number;
  googleUserCount: number;
}

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit {

  public daysStartingFrom = 3;

  @ViewChild('chartPopUp', { static: true }) chartPopUp: TemplateRef<any>;



  constructor(private dialog: MatDialog,
    private locationDataService: LocationDataService) { }

  ngOnInit() {
    this.locationDataService.locationData(this.daysStartingFrom).then(data => this.drawChart(data));

  }

  dateRangeChange() {
    this.locationDataService.locationData(this.daysStartingFrom).then(data => this.drawChart(data));
  }

  openDialog(countRegistered: number, title: string, element, location:string) {

    const dialogConfig = new MatDialogConfig();

    dialogConfig.disableClose = true;
    dialogConfig.hasBackdrop = false;
    dialogConfig.autoFocus = false;
    dialogConfig.closeOnNavigation = true;

    const rect = element.getBoundingClientRect();

    dialogConfig.position = {
      left: (rect.left + (rect.width / 2) - 100) + 'px',
      top: (rect.top) + 'px'
    };

    dialogConfig.data = {
      description: title,
      count: countRegistered,
      location: location
    };



    const newPopUp = this.dialog.open(this.chartPopUp, dialogConfig);
    dialogConfig.position.top = (parseInt(dialogConfig.position.top, 10) - 35 -
      (newPopUp._containerInstance as any)._elementRef.nativeElement.offsetHeight) + 'px';
    newPopUp.updatePosition(dialogConfig.position);



  }

  drawChart(data: Location[]) {


    // set the dimensions and margins of the graph
    const margin = { top: 20, right: 20, bottom: 30, left: 100 };

    const graphic = document.getElementById('graphic').getBoundingClientRect();

    const width = graphic.width - margin.left - margin.right;
    const height = graphic.height - margin.top - margin.bottom;




    // data = data.slice(1, 5);

    data.sort((a, b) => {
      const sumA = a.facebookUserCount + a.googleUserCount + a.shelterUserCount;
      const sumB = b.facebookUserCount + b.googleUserCount + b.shelterUserCount;

      if (sumA < sumB) {
        return -1;
      } else if (sumA > sumB) {
        return 1;
      } else {
        return 0;
      }

    });

    // set the ranges
    const y = d3.scaleBand()
      .range([height - 200, 0])
      .padding(0.1);

    const yRight = d3.scaleLinear()
      .range([height - 200, 0]);


    const x = d3.scaleLinear()
      .range([0, width]);

    // append the svg object to the body of the page
    // append a 'group' element to 'svg'
    // moves the 'group' element to the top left margin
    let svg = d3.select('#graphic').select('svg');

    if (svg.empty()) {
      svg = d3.select('#graphic').append('svg')
        .attr('width', width + margin.left + margin.right)
        .attr('height', height + margin.top + margin.bottom)
        .append('g')
        .attr('transform',
          'translate(' + margin.left + ',' + margin.top + ')');
    }

    const attachTooltip = (bar: any, title: string, countField: string) => {
      bar.on('mouseover', (d, i, bars) => {
        this.openDialog(d[countField], title, bars[i], d.location);
      });
      bar.on('mouseout', () =>
        this.dialog.closeAll()
      );
    };

    const median = data.reduce<number>((acc, cur) => {
      return acc += cur.facebookUserCount + cur.googleUserCount + cur.shelterUserCount;
    }, 0) / data.length;



    // Scale the range of the data in the domains
    x.domain([0, d3.max(data, (d) => d.inviteCount)]);
    yRight.domain([0, d3.max(data, (d) => d.inviteCount)]);
    y.domain(data.map((d) => d.location));

    const xAxisBottom = d3.axisBottom(x).tickFormat((d: number) => {
      return d3.format('.0%')(d / x.domain()[1]);
    });
    const xAxisTop = d3.axisTop(x).tickFormat((d: number) => {
      return d3.format('.0%')(d / x.domain()[1]);
    });
    const yAxis = d3.axisLeft(y);
    const yAxisRight = d3.axisRight(yRight).tickFormat((d: number) => {
      if (d === 0) {
        return '';
      }
      return d3.format('.0%')(d / x.domain()[1]);
    }).tickValues([0, x.domain()[1] * 0.15, x.domain()[1] * 0.30, x.domain()[1] * 0.45,
      x.domain()[1] * 0.60, x.domain()[1] * 0.75, x.domain()[1] * 0.90]);



    // append the rectangles for the bar chart
    const facebookBars = svg.selectAll('.bar_facebook')
      .data(data);


    // Drawing new rect with starting width 0
    facebookBars.enter().append('rect')
      .attr('y', (d) => y(d.location))
      .attr('height', y.bandwidth())
      .attr('class', 'bar_facebook')
      .transition().duration(1000).ease(d3.easeLinear)
      .attr('width', (d) => x(d.facebookUserCount));

    // Animates changes to existing bars
    facebookBars
      .transition().duration(1000).ease(d3.easeLinear)
      .attr('width', (d) => x(d.facebookUserCount));

    // Removes bars no longer necessary
    facebookBars.exit()
      .transition().duration(1000).ease(d3.easeLinear)
      .attr('width', 0)
      .remove();

    attachTooltip(svg.selectAll('.bar_facebook'), 'Facebook users', 'facebookUserCount');

    const googleBars = svg.selectAll('.bar_google')
      .data(data);

    googleBars.enter().append('rect')
      .attr('class', 'bar_google')
      .attr('x', d => x(d.facebookUserCount))
      .attr('y', (d) => y(d.location))
      .attr('height', y.bandwidth())
      .attr('width', 0)
      .transition().duration(1000).ease(d3.easeLinear).delay(1000)
      .attr('width', (d) => x(d.googleUserCount));

    // Animates changes to existing bars
    googleBars
      .transition().duration(1000).delay(1000).ease(d3.easeLinear)
      .attr('width', (d) => x(d.googleUserCount))
      .attr('x', d => x(d.facebookUserCount));

    // Removes bars no longer necessary
    googleBars.exit()
      .transition().duration(1000).delay(1000).ease(d3.easeLinear)
      .attr('width', 0)
      .remove();


    attachTooltip(svg.selectAll('.bar_google'), 'Google users', 'googleUserCount');


    const shelterBars = svg.selectAll('.bar_shelter')
      .data(data);

    shelterBars.enter().append('rect')
      .attr('class', 'bar_shelter')
      .attr('x', d => x(d.facebookUserCount) + x(d.googleUserCount))
      .attr('y', (d) => y(d.location))
      .attr('height', y.bandwidth())
      .attr('width', 0)
      .transition().duration(1000).ease(d3.easeLinear).delay(2000)
      .attr('width', (d) => x(d.shelterUserCount));

    // Animates changes to existing bars
    shelterBars
      .transition().duration(1000).delay(2000)
      .ease(d3.easeLinear)
      .attr('width', (d) => x(d.shelterUserCount))
      .attr('x', d => x(d.facebookUserCount) + x(d.googleUserCount));

    // Removes bars no longer necessary
    shelterBars.exit()
      .transition().duration(1000).delay(2000)
      .ease(d3.easeLinear)
      .attr('width', 0)
      .remove();

    attachTooltip(svg.selectAll('.bar_shelter'), 'Shelter users', 'shelterUserCount');


    const winnerCircle = svg.selectAll('.bar_winner')
      .data(data);

    winnerCircle.enter().append('circle')
      .attr('cx', d => x(d.facebookUserCount) + x(d.googleUserCount) + x(d.shelterUserCount) + 20)
      .attr('cy', (d) => y(d.location) + (y.bandwidth() / 2))
      .attr('class', (d) => {
        const win = d3.max([d.facebookUserCount, d.googleUserCount, d.shelterUserCount]);
        if (win === d.facebookUserCount) {
          return 'bar_winner bar_facebook';
        } else if (win === d.googleUserCount) {
          return 'bar_winner bar_google';
        } else {
          return 'bar_winner bar_shelter';
        }
      })
      .attr('r', 0)
      .transition().duration(1000).ease(d3.easeLinear).delay(3000)
      .attr('r', 7);

    winnerCircle
      .attr('r', 0)
      .transition().duration(1000).delay(3000)
      .ease(d3.easeLinear)
      .attr('r', 7)
      .attr('class', (d) => {
        const win = d3.max([d.facebookUserCount, d.googleUserCount, d.shelterUserCount]);
        if (win === d.facebookUserCount) {
          return 'bar_winner bar_facebook';
        } else if (win === d.googleUserCount) {
          return 'bar_winner bar_google';
        } else {
          return 'bar_winner bar_shelter';
        }
      })
      .attr('cy', (d) => y(d.location) + (y.bandwidth() / 2))
      .attr('cx', d => 20 + x(d.shelterUserCount) + x(d.facebookUserCount) + x(d.googleUserCount));

    const medianBar = svg.selectAll('.bar_median')
      .data([median]);

    medianBar.enter().append('rect')
      .attr('x', (d) => x(d))
      .attr('height', height - 200)
      .attr('class', 'bar_median')
      .attr('y', 0)
      // .transition().duration(1000).ease(d3.easeLinear)
      .attr('width', 10);

    medianBar
      .transition().duration(1000).ease(d3.easeLinear)
      .attr('x', (d) => x(d));

    // add the x Axis
    if (svg.select('.chart_axisBottom').empty()) {
      svg.append('g')
        .attr('class', 'chart_axisBottom')
        .attr('transform', 'translate(0,' + (height - 200) + ')')
        .call(xAxisBottom);

      svg.append('g')
        .attr('class', 'chart_axisTop')
        .attr('transform', 'translate(0,0)')
        .call(xAxisTop);

    } else {
      svg.select('.chart_axisBottom').transition().duration(1000).ease(d3.easeLinear).call(xAxisBottom.bind(this));
      svg.select('.chart_axisTop').transition().duration(1000).ease(d3.easeLinear).call(xAxisTop.bind(this));
    }

    // add the y Axis
    if (svg.select('.chart_axisLeft').empty()) {
      svg.append('g')
        .attr('class', 'chart_axisLeft')
        .call(yAxis);
      svg.append('g')
        .attr('class', 'chart_axisRight')
        .attr('transform', 'translate(' + (width - 25) + ',0)')
        .call(yAxisRight);
    } else {
      svg.select('.chart_axisLeft').transition().duration(1000).ease(d3.easeLinear).call(yAxis.bind(this));
      svg.select('.chart_axisRight').transition().duration(1000).ease(d3.easeLinear).call(yAxisRight.bind(this));
    }

    const legendRectSize = 18;
    const legendSpacing = 4;

    const legend = svg.selectAll('.legend')
      .data(['Facebook', 'Google', 'Shelter'])
      .enter()
      .append('g')
      .attr('class', 'legend')
      .attr('transform', (d, i) => {
        return 'translate(' + (((width - 400) / 2) + (i * 130)) + ',' + (height - 160) + ')';
      });

    legend.append('rect')
      .attr('width', legendRectSize)
      .attr('height', legendRectSize)
      .attr('class', (d) => {
        if (d === 'Facebook') {
          return 'legend_facebook';
        } else if (d === 'Google') {
          return 'legend_google';
        } else {
          return 'legend_shelter';
        }
      });
    legend.append('text')
      .attr('x', legendRectSize + legendSpacing)
      .attr('y', legendRectSize - legendSpacing)
      .attr('class', 'legend_text')
      .text((d) => d);
  }

}
