import {Component, inject, OnInit, output} from "@angular/core";
import {CategoryService} from "../../services";
import {Category} from "../../models";

@Component({
  selector: 'app-category-list',
  templateUrl: './category-list.component.html',
  styleUrls: ['./category-list.component.scss']
})
export class CategoryListComponent implements OnInit {
  categorySelected = output<string>();
  private categoryService = inject(CategoryService);
  categories: Category[] = [];

  ngOnInit() {
    this.getPublicCategories();
  }

  onCategorySelected(categoryName: string) {
    this.categorySelected.emit(categoryName);
  }

  private getPublicCategories = () => {
    this.categoryService.getPublicCategories().subscribe(categories => {
      this.categories = categories.items || [];
    })
  }
}
