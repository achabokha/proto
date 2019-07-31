import { IStyles, ITheme } from "./interfaces.theme";

export class EAlliedTheme implements ITheme {
    name: string = 'e-allied';
    domain: string = 'e-allied.com';
    showServiceAlert: boolean = true;
    showSignUpButton: boolean = false;
    showCardProgramDisclaimer: boolean = true;
    showAppJustSubmitButtot: boolean = false;
    cardName: string = "E-Allied Card";
    styles: IStyles = {
        '--card-card-number-color': 'whitesmoke',
        '--navbar-background-color': '#f8f9fa',
        '--card-image': 'url(/assets/images/themes/e-allied/card.png)',
    }
}
