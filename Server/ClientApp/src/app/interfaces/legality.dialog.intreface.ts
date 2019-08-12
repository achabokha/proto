
import {ICredentials} from './main.interface';
import { AuthProvider } from '../enums';

export interface LegalityDialogParams {
  tosUrl: string;
  privacyPolicyUrl: string;
  authProvider: AuthProvider;
  credentials?: ICredentials
}

export interface LegalityDialogResult {
  checked: boolean;
  authProvider: AuthProvider;
  credentials?: ICredentials
}