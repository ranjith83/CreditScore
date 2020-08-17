import { TestBed, async, inject } from '@angular/core/testing';

import { YGuard } from './y.guard';

describe('YGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [YGuard]
    });
  });

  it('should ...', inject([YGuard], (guard: YGuard) => {
    expect(guard).toBeTruthy();
  }));
});
