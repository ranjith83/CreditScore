import { Role } from "./role";

export class User {
    id: number;
    username: string;
    password: string;
    firstName: string;
  lastName: string;
  companyId: number;
    role: Role;
    token?: string;
}

