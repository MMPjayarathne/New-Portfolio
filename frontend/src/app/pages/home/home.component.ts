import { Component } from '@angular/core';
import { NavBarComponent } from '../../components/nav-bar/nav-bar.component';
import { MainComponent } from '../../components/main/main.component';
import { AboutMeComponent } from '../../components/about-me/about-me.component';
import { MySkillsComponent } from '../../components/my-skills/my-skills.component';
import { MyProjectsComponent } from '../../components/my-projects/my-projects.component';
import { ContactMeComponent } from '../../components/contact-me/contact-me.component';
import { BlogsComponent } from '../../components/blogs/blogs.component';
import { FooterComponent } from '../../components/footer/footer.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [NavBarComponent,MainComponent,AboutMeComponent,MySkillsComponent,MyProjectsComponent,ContactMeComponent,BlogsComponent,FooterComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss'
})
export class HomeComponent {

}

