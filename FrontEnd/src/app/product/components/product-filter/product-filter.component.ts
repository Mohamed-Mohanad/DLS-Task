import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Category } from '../../../core/models/product.model';

@Component({
  selector: 'app-product-filter',
  templateUrl: './product-filter.component.html',
  styleUrls: ['./product-filter.component.scss'],
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatFormFieldModule,
    MatSelectModule,
    MatButtonModule,
    MatIconModule
  ]
})
export class ProductFilterComponent implements OnInit {
  @Input() categories: Category[] = [];
  @Output() filterChange = new EventEmitter<{categoryId?: number}>();
  
  filterForm!: FormGroup;
  
  constructor(private fb: FormBuilder) {}
  
  ngOnInit(): void {
    this.initForm();
  }
  
  initForm(): void {
    this.filterForm = this.fb.group({
      categoryId: [null]
    });
    
    this.filterForm.valueChanges.subscribe(filters => {
      this.filterChange.emit(filters);
    });
  }
  
  resetFilters(): void {
    this.filterForm.reset({
      categoryId: null
    });
  }
} 