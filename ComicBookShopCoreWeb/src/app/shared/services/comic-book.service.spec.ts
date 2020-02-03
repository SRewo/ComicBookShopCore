import {getTestBed, TestBed} from '@angular/core/testing';

import { ComicBookService } from './comic-book.service';
import {ComicBook} from '../models/comic-book';
import {of} from 'rxjs';
import {HttpErrorResponse} from '@angular/common/http';
import {HttpClientTestingModule, HttpTestingController} from '@angular/common/http/testing';

describe('ComicBookService', () => {
  let injector: TestBed;
  let service: ComicBookService;
  let httpMock: HttpTestingController;

  const testData = {
    data: [{
      id: 1,
      title: 'Batman #1',
      shortDescription: 'Desc',
      price: 20,
      publisher: 'DC Comics',
      onSaleDate: '01-01-2020'
  }]
  };

  const mockErrorResponse = { status: 400, statusText: 'Bad Request' };

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [ComicBookService]
    });

    injector = getTestBed();
    service = injector.get(ComicBookService);
    httpMock = injector.get(HttpTestingController);
  });

  afterEach(() => httpMock.verify());

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('getAllComics should return data', () => {
      service.getAllComics().subscribe(
        com => expect(com).toEqual(testData)
      );
      const request = httpMock.expectOne('http://localhost:8081/api/comicbook');
      expect(request.request.method).toBe('GET');
      request.flush(testData);
  });

  it('getAllComics should properly display error', () => {
    let response;
    let data;
    service.getAllComics().subscribe(
      res => data = res,
      err => response = err
    );
    httpMock.expectOne('http://localhost:8081/api/comicbook').flush(null, mockErrorResponse);
    expect(response.status).toBe(400);
  });
});
