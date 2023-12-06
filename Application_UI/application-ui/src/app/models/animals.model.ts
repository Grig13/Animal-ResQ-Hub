import {Sizes} from "./enums/sizes.enums";
import {AnimalGenders} from "./enums/genders.enums";
import {GoodWith} from "./enums/good-with.enums";
import {CoatTypes} from "./enums/coat-types.enums";
import {Shelters} from "./shelters.model";

export class Animals{
  id ?: string;
  type !: string;
  breed ?: string;
  name ?: string;
  age ?: number;
  size ?: Sizes;
  health ?: string;
  gender ?: AnimalGenders;
  goodWith ?: GoodWith;
  coatLength ?: CoatTypes;
  specialTrained ?: boolean;
  shelter ?: Shelters;
}
