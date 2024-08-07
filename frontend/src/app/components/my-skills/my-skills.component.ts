import { Component } from '@angular/core';
import { FlexLayoutModule } from '@angular/flex-layout';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-my-skills',
  standalone: true,
  imports: [FlexLayoutModule,CommonModule],
  templateUrl: './my-skills.component.html',
  styleUrl: './my-skills.component.scss'
})
export class MySkillsComponent {
  selectedTab: string = 'Languages';

  selectTab(tab: string) {
    this.selectedTab = tab;
  }

}
