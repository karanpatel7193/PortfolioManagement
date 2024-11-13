import { ResponseModel } from "./response.model";

export class UserModel {
    id: number = 0;
    first_name: string = '';
    last_name: string = '';
    email: string = '';
    avatar: string = '';
    username: string = '';
    is_admin: boolean = false;
    is_SFTP_Enabled: boolean = false;
    date_joined: Date | null = null;
    last_login: Date | null = null;
    prev_login: Date | null = null;
    authToken: string | null = null;
    
    password: string = '';

}

export class UserLoginParameter {
    email: string = '';
    password: string = '';
    captcha: string|null = null;
}

export class LoginResponseModel extends ResponseModel {
    token: string = '';
}
