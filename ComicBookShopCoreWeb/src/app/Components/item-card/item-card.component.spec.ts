import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemCardComponent } from './item-card.component';
import {ComicBook} from '../../shared/models/comic-book';

describe('ItemCardComponent', () => {
  let component: ItemCardComponent;
  let fixture: ComponentFixture<ItemCardComponent>;
  const comic: ComicBook = {
    id: 1,
    title: 'Batman #1',
    shortDescription: 'Desc',
    price: 20,
    publisherName: 'DC Comics',
    onSaleDate: '01-01-2020'
  };

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ItemCardComponent ],
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ItemCardComponent);
    component = fixture.componentInstance;
    component.comic = comic;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeDefined();
  });
});
