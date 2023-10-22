import { ChangeDetectionStrategy, Component, HostBinding, Input } from "@angular/core";
import { MultidimensionalArray } from "../multidimentional-array";

@Component({
    selector: "multidimentional-array",
    templateUrl: "./multidimentional-array.component.html",
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class MultidimensionalViewComponent<T> {
    @Input()
    value: MultidimensionalArray<T> = [];

    @HostBinding("class._array")
    get isArray(): boolean {
        return Array.isArray(this.value);
    }
}