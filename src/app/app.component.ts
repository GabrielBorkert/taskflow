import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  // você pode deixar o template inline mesmo:
  template: `<router-outlet></router-outlet>`,
})
export class AppComponent {}