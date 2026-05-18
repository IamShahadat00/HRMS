import { TestBed } from '@angular/core/testing';

import { Aut } from './aut';

describe('Aut', () => {
  let service: Aut;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(Aut);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
