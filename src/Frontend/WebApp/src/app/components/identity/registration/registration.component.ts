import { Component } from "@angular/core";
import { FormBuilder, FormGroup, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { IRegistration } from "src/app/models/identity/registration";
import { IdentityService } from "src/app/services/identity.service";

@Component({
    selector: 'app-registration',
    templateUrl: './registration.component.html'
})
export class RegistrationComponent {
    form: FormGroup;

    constructor(private fb: FormBuilder,
        private identityService: IdentityService,
        private router: Router) {

        this.form = this.fb.group({
            username: ['', Validators.required],
            email: ['', Validators.required],
            password: ['', Validators.required]
        });
    }

    submit() {
        const val = this.form.value;

        if (val.username && val.email && val.password) {
            var registration: IRegistration = {username: val.username, email: val.email, password: val.password}
            this.identityService.registration(registration)
                .subscribe(
                    () => {
                        console.log("User is registerd");
                        this.router.navigateByUrl('/');
                    }
                );
        }
    }
}