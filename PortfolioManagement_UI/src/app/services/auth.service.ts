import { Injectable } from '@angular/core';
import { SessionService } from './session.service';

@Injectable()
export class AuthService {
    constructor(private session: SessionService) { }

    public IsAuthenticated(): boolean {
        const user = this.session.getUser();

        // Check whether the token is expired and return
        // true or false
        return (user != null && user.token != null);
    }
}
