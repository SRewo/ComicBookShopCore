import {Component, Input} from '@angular/core';
import {ComicBook} from '../../shared/models/comic-book';

@Component({
  selector: 'app-item-card',
  templateUrl: './item-card.component.html',
  styleUrls: ['./item-card.component.css']
})
export class ItemCardComponent {
  @Input() comic: ComicBook;

  constructor() {}

}

