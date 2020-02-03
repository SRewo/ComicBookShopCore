import { Injectable } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Observable} from 'rxjs';
import {ComicBook} from '../models/comic-book';

@Injectable({
  providedIn: 'root'
})
export class ComicBookService {
   url = 'http://localhost:8081/api/comicbook';
   constructor(private http: HttpClient) {}
   getAllComics(): Observable<any> {
     return this.http.get<ComicBook>(this.url);
}
}
