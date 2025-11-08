import { CommonModule } from '@angular/common';
import { Component, signal } from '@angular/core';
import { HeaderComponent } from './layout/header/header';
import { RouterOutlet } from '@angular/router';
import { FooterComponent } from './layout/footer/footer';

@Component({
  selector: 'app-root',
  imports: [CommonModule, HeaderComponent, RouterOutlet, FooterComponent],
  templateUrl: './app.html',
  styleUrl: './app.css',
})
export class App {
  protected readonly title = signal('CLLIX.TAAuditTracker.ClientUi');
}
