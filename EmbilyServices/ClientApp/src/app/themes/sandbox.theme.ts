import { IStyles, ITheme } from "./interfaces.theme";

export class SandboxTheme implements ITheme {
    name: string = 'sandbox';
    domain: string = 'sandbox.embily.com';
    showServiceAlert: boolean = false;
    showSignUpButton: boolean = true;
    showCardProgramDisclaimer: boolean = false;
    showAppJustSubmitButtot: boolean = true;
    cardName: string = "Sandbox Card";
    styles: IStyles = {
        '--card-card-number-color': 'whitesmoke',
        '--navbar-background-color': '#f8f9fa',
        '--card-image': 'url(/assets/images/themes/sandbox/card.png)',
    }
}
