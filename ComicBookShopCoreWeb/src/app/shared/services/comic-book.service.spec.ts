import { TestBed } from '@angular/core/testing';

import { ComicBookService } from './comic-book.service';

describe('ComicBookService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: ComicBookService = TestBed.get(ComicBookService);
    expect(service).toBeTruthy();
  });
});
