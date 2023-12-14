import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';

interface Dog {
  dogName: string;
  breed: string;
  age: number;
  color: string;
  editing: boolean;
}

@Component({
  selector: 'app-dogs',
  templateUrl: './dogs.component.html',
  styleUrls: ['./dogs.component.css']
})
export class DogsComponent implements OnInit {
  dogArray: Dog[] = [{ dogName: 'Buddy', breed: 'Labrador Retriever', age: 3, color: 'Golden', editing: false }];

  newDog: Dog = {
    dogName: '',
    breed: '',
    age: 0,
    color: '',
    editing: false
  };

  constructor() { }

  ngOnInit(): void {
  }

  onSubmit(form: NgForm) {
    if (this.newDog.editing) {
      // Update existing dog
      const index = this.dogArray.findIndex(dog => dog.editing);
      this.dogArray[index] = { ...this.newDog, editing: false };
    } else {
      // Add new dog
      this.dogArray.push({ ...this.newDog, editing: false });
    }

    form.reset();
  }

  onDelete(index: number) {
    this.dogArray.splice(index, 1);
  }

  onEdit(index: number) {
    // Set editing to true for the selected dog
    this.dogArray.forEach((dog, i) => {
      dog.editing = i === index;
    });

    // Populate the form with the selected dog's details
    this.newDog = { ...this.dogArray[index], editing: true };
  }

  onCancelEdit() {
    // Reset the form and editing status
    this.newDog = { dogName: '', breed: '', age: 0, color: '', editing: false };
  }
}
