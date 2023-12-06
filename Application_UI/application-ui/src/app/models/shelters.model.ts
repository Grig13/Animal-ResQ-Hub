import {SheltersLocations} from "./enums/shelters-locations.enums";

export class Shelters{
  id ?: string;
  shelterName ?: string;
  shelterDescription ?: string;
  shelterLocation ?: SheltersLocations;
  capacity ?: number;
  shelterAddress ?: string;
}
