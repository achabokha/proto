import { IStyles, ITheme } from "./interfaces.theme";

export class EmbilyTheme implements ITheme {
    name: string = 'embily';
    domain: string = 'services.embily.com';
    showServiceAlert: boolean = false;
    showSignUpButton: boolean = false;
    showCardProgramDisclaimer: boolean = false;
    showAppJustSubmitButtot: boolean = false;
    cardName: string = 'Embily Card';
    styles: IStyles = {
        '--card-card-number-color': 'black',
        '--navbar-background-color': 'white',
        '--card-image': 'url(/assets/images/themes/embily/card.png)',
    };
}
