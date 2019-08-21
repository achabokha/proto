import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-commentform',
  templateUrl: './commentform.component.html',
  styleUrls: ['./commentform.component.scss']
})
export class CommentformComponent implements OnInit {
  @Input()reply: boolean;

  constructor() { }

  ngOnInit() {
  }

}
