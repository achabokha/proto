import { IStyles, ITheme } from "./interfaces.theme";

export class FilixTheme implements ITheme {
    name: string = 'filix';
    domain: string = 'embilyservices2.azurewebsites.net';
    showServiceAlert: boolean = false;
    showSignUpButton: boolean = true;
    showCardProgramDisclaimer: boolean = false;
    showAppJustSubmitButtot: boolean = false;
    cardName: string = 'Lao International Bank Card';
    styles: IStyles = {
        '--card-card-number-color': 'black',
        '--navbar-background-color': 'white',
        '--card-image': 'url(/assets/images/themes/filix/card.png)',
    };
}
