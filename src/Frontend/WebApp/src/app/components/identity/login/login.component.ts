import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { ILoginRequest } from "src/app/models/identity/login.request";
import { IdentityService } from "src/app/services/identity.service";

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html'
})
export class LoginComponent {
    form: FormGroup;

    constructor(private fb: FormBuilder,
        private identityService: IdentityService,
        private router: Router) {

        this.form = this.fb.group({
            login: ['', Validators.required],
            password: ['', Validators.required],
            rememberLogin: [false]
        });
    }

    submit() {
        const val = this.form.value;

        if (val.login && val.password) {
            var login: ILoginRequest = {login: val.login, password: val.password, rememberLogin: val.rememberLogin}
            this.identityService.login(login)
                .subscribe(
                    () => {
                        console.log("User is logged in");
                        this.router.navigateByUrl('/');
                    }
                );
        }
    }
}