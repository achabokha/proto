export interface IStyles {
    '--card-card-number-color': string,
    '--navbar-background-color': string;
    '--card-image': string;
}

export interface ITheme {
    name: string;
    domain: string;
    showServiceAlert: boolean;
    showSignUpButton: boolean;
    showCardProgramDisclaimer: boolean;
    // show on Application Payment screen Just Submit button to avoid making a payment. Needed for Sandbox env
    // TODO: it is a bad idea, need a better solution. A theme should not have environment specific settings --
    showAppJustSubmitButtot: boolean; 
    cardName: string;
    styles: IStyles;
}
