export enum TaskPriority 
{
    MuitoAlta = 1,
    Alta = 2,
    Media = 3,
    Baixa = 3,
    MuitoBaixa = 5,
}

export interface Priority
{
  id: number,
  name: string,
  color: string,
  displayOrder: number,
}