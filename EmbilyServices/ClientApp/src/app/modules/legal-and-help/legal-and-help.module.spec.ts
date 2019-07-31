import { LegalAndHelpModule } from './legal-and-help.module';

describe('LegalAndHelpModule', () => {
  let legalAndHelpModule: LegalAndHelpModule;

  beforeEach(() => {
    legalAndHelpModule = new LegalAndHelpModule();
  });

  it('should create an instance', () => {
    expect(legalAndHelpModule).toBeTruthy();
  });
});
