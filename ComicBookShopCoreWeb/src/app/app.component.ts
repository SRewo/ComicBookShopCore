import {Component, OnInit} from '@angular/core';
import {ComicBookService} from './shared/services/comic-book.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'ComicBookShopCoreWeb';
  comics;

  constructor(private service: ComicBookService) {
  }

  ngOnInit(): void {
    this.service.getAllComics().subscribe((data) => {
      this.comics = data;
    } );
 }
}
