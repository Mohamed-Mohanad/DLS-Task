import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatTableModule } from '@angular/material/table';
import { CartComponent } from './components/cart/cart.component';
import { CartService } from './services/cart.service';

@NgModule({
    declarations: [],
    imports: [
        CommonModule,
        ReactiveFormsModule,
        MatButtonModule,
        MatCardModule,
        MatIconModule,
        MatInputModule,
        MatTableModule,
        CartComponent,
        RouterModule.forChild([
            { path: '', component: CartComponent }
        ])
    ],
    providers: [
        CartService
    ]
})
export class CartModule { } 