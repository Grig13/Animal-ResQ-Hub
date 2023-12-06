import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {Animals} from "../../app/models/animals.model";
import {Injectable} from "@angular/core";

@Injectable({
  providedIn: 'root',
})

export class AnimalsService {
  private apiUrl = 'https://localhost:7125';

  constructor(private http: HttpClient) {}

  getAllAnimals(): Observable<Animals> {
    return this.http.get<Animals>(`${this.apiUrl}/animal/all-animals`);
  }

  getAnimalById(id: string): Observable<Animals> {
    return this.http.get<Animals>(`${this.apiUrl}/animal/animal-by-id?id=${id}}`);
  }

  addAnimal(animal: Animals): Observable<Animals>{
    return this.http.post<Animals>(`${this.apiUrl}/animal`, animal);
  }

  editAnimal(animal: Animals): Observable<Animals>{
    return this.http.put<Animals>(`${this.apiUrl}/animal`, animal);
  }

  deleteAnimal(id: string): Observable<Animals>{
    return this.http.delete<Animals>(`${this.apiUrl}/animal/${id}`);
  }
}
