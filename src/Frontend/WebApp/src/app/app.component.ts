import { Component } from '@angular/core';
import { MultidimensionalArray } from './extentions/multidimentional-array/multidimentional-array';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'WebApp';

  readonly items: MultidimensionalArray<string> = [
    "Hello",
    ["here", "is", ["some", "structured"], "Data"],
    "Bye"
];


}
