import { Component, OnInit } from '@angular/core';
import {ComicBookService} from '../../shared/services/comic-book.service';

@Component({
  selector: 'app-main-page',
  templateUrl: './main-page.component.html',
  styleUrls: ['./main-page.component.css']
})
export class MainPageComponent implements OnInit {
  comics;

  constructor(private service: ComicBookService) { }

  ngOnInit() {
    this.service.getAllComics().subscribe(x => this.comics = x);
  }

}
