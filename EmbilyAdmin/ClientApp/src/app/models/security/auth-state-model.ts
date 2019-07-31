import { AuthTokenModel } from './auth-token-model';
import { ProfileModel } from './profile-model';

//export interface AuthStateModel {
//  tokens?: AuthTokenModel;
//  profile?: ProfileModel;
//  authReady?: boolean;
//}
export interface AuthStateModel {
    tokens?: AuthTokenModel;
    profile?: ProfileModel;
    authReady?: boolean;
}
