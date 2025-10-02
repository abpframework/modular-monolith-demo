import {Pipe, PipeTransform} from "@angular/core";
import {environment} from "../../environments/environment";

@Pipe({
  name: 'absoluteImageUrl'
})
export class AbsoluteImageUrlPipe implements PipeTransform {
  transform(productId: string): string {
    if (!productId) {
      return '';
    } else {
      return `${environment.apis.default.url}/api/catalog/public/products/image/${productId}`;
    }
  }
}
