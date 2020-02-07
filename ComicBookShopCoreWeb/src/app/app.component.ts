import {Component, OnInit} from '@angular/core';
import {ComicBookService} from './shared/services/comic-book.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ComicBookShopCoreWeb';

  constructor() {
  }
}
